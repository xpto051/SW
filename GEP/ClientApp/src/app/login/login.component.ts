import { Component, OnInit, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { NgForm } from "@angular/forms";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"],
})
export class LoginComponent implements OnInit {
  formModel = {
    Email: "",
    Password: "",
  };
  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private router: Router,
    private toastr: ToastrService
  ) {}

  ngOnInit() {
    if (localStorage.getItem("token") != null) {
      this.router.navigateByUrl("/gep/home");
    }
  }

  onSubmit(form: NgForm) {
    var url = this.baseUrl + "api/user/login";
    this.http.post(url, form.value).subscribe(
      (res: any) => {
        localStorage.setItem("token", res.token);
        this.router.navigateByUrl("/gep/home");
      },
      (err) => {
        if (err.status == 400) {
          console.log("chegou aqui");
          this.toastr.error(
            "O email ou a password não estão corretos.",
            "Authentication failed."
          );
        } else {
          console.log(err);
        }
      }
    );
  }
}
