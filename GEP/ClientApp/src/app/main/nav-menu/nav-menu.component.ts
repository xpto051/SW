import { Component } from "@angular/core";
import { Router } from "@angular/router";

@Component({
  selector: "app-nav-menu",
  templateUrl: "./nav-menu.component.html",
  styleUrls: ["./nav-menu.component.css"],
})
export class NavMenuComponent {
  isExpanded = false;
  role = null;

  constructor(private router: Router) {
    var payload = JSON.parse(
      window.atob(localStorage.getItem("token").split(".")[1])
    );
    this.role = payload.role;
    console.log(this.role);
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
