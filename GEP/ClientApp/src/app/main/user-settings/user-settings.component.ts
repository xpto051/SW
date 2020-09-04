import { Component, OnInit, Inject } from "@angular/core";
import { Router } from "@angular/router";
import { HttpClient, HttpHeaders } from "@angular/common/http";

@Component({
  selector: "app-user-settings",
  templateUrl: "./user-settings.component.html",
  styleUrls: ["./user-settings.component.css"],
})
export class UserSettingsComponent implements OnInit {
  formDataStudent = {
    Number: 0,
    FirstName: "",
    LastName: "",
    PhoneNumber: null,
    email: "",
    password: "",
  };
  role = null;

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

  ngOnInit() {}
}
