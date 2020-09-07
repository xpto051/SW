import { BrowserModule } from "@angular/platform-browser";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
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
  MatTableModule,
  MatSelectModule,
  MatDialogModule,
  MAT_DIALOG_DEFAULT_OPTIONS,
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
import { UserSettingsComponent } from "./main/user-settings/user-settings.component";
import { CompanyDetailsComponent } from './main/company-details/company-details.component';
//import { CompanyDetailsTemplateComponent } from './main/company-details/company-details.component';
import { StudentsListComponent } from "./main/students-list/students-list.component";
import { PerfectScrollbarModule, PERFECT_SCROLLBAR_CONFIG, PerfectScrollbarConfigInterface } from "ngx-perfect-scrollbar";

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
  wheelPropagation: true,
};

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
    UserSettingsComponent,
    CompanyDetailsComponent,
    StudentsListComponent,
    //CompanyDetailsTemplateComponent,
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
    MatTableModule,
    MatDialogModule,
    MatSelectModule,
    BrowserAnimationsModule,
    PerfectScrollbarModule,
    ReactiveFormsModule,
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
            path: "addUser",
            component: AddUserComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
          { path: "viewStudents", component: StudentsListComponent },
          { path: "viewCompanies", component: CompanyDetailsComponent },
          { path: "settings", component: UserSettingsComponent },
        ],
      },
    ]),
  ],
  providers: [
    {
      provide: PERFECT_SCROLLBAR_CONFIG,
      useValue: DEFAULT_PERFECT_SCROLLBAR_CONFIG,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },
    {
      provide: MAT_DIALOG_DEFAULT_OPTIONS,
      useValue: { hasBackdrop: false },
    },
  ],
  bootstrap: [AppComponent],
  //entryComponents: [CompanyDetailsComponent, CompanyDetailsTemplateComponent]
})
export class AppModule { }
