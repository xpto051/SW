import { Component, OnInit, Inject } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";
import { NgForm } from "@angular/forms";

@Component({
  selector: "app-user-settings",
  templateUrl: "./user-settings.component.html",
  styleUrls: ["./user-settings.component.css"],
})
export class UserSettingsComponent implements OnInit {
  formData: CompanyRespDetails;
  role = null;

  constructor(
    private router: Router,
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) {
    var payload = JSON.parse(
      window.atob(localStorage.getItem("token").split(".")[1])
    );
    this.role = payload.role;
  }

  ngOnInit() {
    this.resetForm();
  }

  putRespUser(formData: CompanyRespDetails) {
    var url = this.baseUrl + "api/CompanyResps";
    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });
    return this.http.put(url, formData, { headers: token }).subscribe(
      (res) => {
        this.resetForm();
        this.toastr.success("Perfil atualizado com sucesso");
      },
      (err) => {
        console.log(err);
      }
    );
  }

  onSubmit(form: NgForm) {
    if (form.value.password == "") {
      form.value.password = null;
    }
    this.putRespUser(form.value);
  }

  resetForm(form?: NgForm) {
    var url = this.baseUrl + "api/CompanyResps/myDetails";
    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });
    this.http
      .get<CompanyRespDetails>(url, { headers: token })
      .subscribe(
        (res) => {
          if (form != null) form.resetForm();
          this.formData = {
            firstName: res.firstName,
            lastName: res.lastName,
            phoneNumber: res.phoneNumber,
            password: null,
            newPassword: null,
            confirmNewPassword: null,
          };
          console.log(this.formData);
        },
        (err) => {
          console.log(err);
        }
      );
  }
}

export class CompanyRespDetails {
  firstName: string;
  lastName: string;
  phoneNumber: string;
  password: string;
  newPassword: string;
  confirmNewPassword: string;
}
