import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { RouterModule } from "@angular/router";
import {
  MatCardModule,
  MatTabsModule,
  MatFormFieldModule,
  MatInputModule,
  MatButtonModule,
  MatCheckboxModule,
  MatIconModule,
} from "@angular/material";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./main/nav-menu/nav-menu.component";
import { HomeComponent } from "./main/home/home.component";
import { CounterComponent } from "./main/counter/counter.component";
import { FetchDataComponent } from "./main/fetch-data/fetch-data.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { LoginComponent } from "./login/login.component";
import { MainComponent } from "./main/main.component";
import { ToastrModule } from "ngx-toastr";
import { AuthGuard } from "./auth/auth.guard";
import { AuthInterceptor } from "./auth/auth.interceptor";
import { AddUserComponent } from "./main/add-user/add-user.component";
import { ForbiddenComponent } from "./main/forbidden/forbidden.component";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    LoginComponent,
    MainComponent,
    AddUserComponent,
    ForbiddenComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: "ng-cli-universal" }),
    HttpClientModule,
    FormsModule,
    MatInputModule,
    MatCardModule,
    MatTabsModule,
    MatFormFieldModule,
    MatButtonModule,
    MatCheckboxModule,
    MatIconModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      positionClass: "toast-top-right",
      preventDuplicates: true,
    }),
    RouterModule.forRoot([
      { path: "", redirectTo: "/login", pathMatch: "full" },
      { path: "login", component: LoginComponent },
      { path: "forbidden", component: ForbiddenComponent },
      {
        path: "gep",
        component: MainComponent,
        canActivate: [AuthGuard],
        children: [
          { path: "home", component: HomeComponent },
          { path: "counter", component: CounterComponent },
          { path: "fetch-data", component: FetchDataComponent },
          {
            path: "add-user",
            component: AddUserComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
        ],
      },
    ]),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
