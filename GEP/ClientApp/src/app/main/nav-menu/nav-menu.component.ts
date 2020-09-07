import { Component, Inject } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
})
export class NavMenuComponent {
  isExpanded = false;
  role = null;

  userDetails;

  constructor(
    private router: Router,
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string
  ) {
    var payload = JSON.parse(
      window.atob(localStorage.getItem("token").split(".")[1])
    );
    this.role = payload.role;
  }

  ngOnInit() {
    var url = this.baseUrl + "api/userprofile";
    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });
    //verifica qual role
    this.http.get(url, { headers: token }).subscribe(
      (res) => {
        this.userDetails = res;
      },
      (err) => {
        console.log(err);
      }
    );
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  onLogout() {
    localStorage.removeItem("token");
    this.router.navigate(["/login"]);
  }
}
