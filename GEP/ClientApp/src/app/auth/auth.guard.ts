import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  UrlTree,
  Router,
} from "@angular/router";

@Injectable({
  providedIn: "root",
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (localStorage.getItem("token") != null) {
      let roles = next.data["permittedRoles"] as Array<string>;

      if (roles) {
        if (roleMatch(roles)) {
          return true;
        } else {
          this.router.navigate(["/forbidden"]);
          return false;
        }
      }
      return true;
    } else {
      this.router.navigate(["/login"]);
      return false;
    }
  }
}

function roleMatch(allowedRoles): boolean {
  var isMatch = false;
  var payload = JSON.parse(
    window.atob(localStorage.getItem("token").split(".")[1])
  );
  var userRole = payload.role;
  allowedRoles.forEach((elem) => {
    if (userRole == elem) {
      isMatch = true;
      return false;
    }
  });
  return isMatch;
}
