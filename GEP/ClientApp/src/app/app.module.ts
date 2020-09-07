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
} from "@angular/material";

import { AppComponent } from "./app.component";
import { NavMenuComponent } from "./main/nav-menu/nav-menu.component";
import { HomeComponent } from "./main/home/home.component";
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { LoginComponent } from "./login/login.component";
import { MainComponent } from "./main/main.component";
import { ToastrModule } from "ngx-toastr";
import { AuthGuard } from "./auth/auth.guard";
import { AuthInterceptor } from "./auth/auth.interceptor";
import { AddUserComponent } from "./main/add-user/add-user.component";
import { ForbiddenComponent } from "./main/forbidden/forbidden.component";
import { UserSettingsComponent } from "./main/user-settings/user-settings.component";
import { CompanyDetailsComponent } from "./main/company-details/company-details.component";
import { StudentsListComponent } from "./main/students-list/students-list.component";
import {
  PerfectScrollbarModule,
  PERFECT_SCROLLBAR_CONFIG,
  PerfectScrollbarConfigInterface,
} from "ngx-perfect-scrollbar";
import { AddCompanyComponent } from "./main/add-company/add-company.component";
import { AddDocenteComponent } from "./main/add-docente/add-docente.component";
import { AddResponsableComponent } from "./main/add-responsable/add-responsable.component";
import { AddCoordinatorComponent } from "./main/add-coordinator/add-coordinator.component";
import { ViewInternshipsComponent } from "./main/view-internships/view-internships.component";
import { ViewIntershipProposalsComponent } from "./main/view-intership-proposals/view-intership-proposals.component";
import { AddInternshipComponent } from "./main/add-internship/add-internship.component";
import { ViewProjectsComponent } from './main/view-projects/view-projects.component';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true,
  wheelPropagation: true,
};

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    LoginComponent,
    MainComponent,
    AddUserComponent,
    ForbiddenComponent,
    UserSettingsComponent,
    CompanyDetailsComponent,
    StudentsListComponent,
    AddCompanyComponent,
    AddDocenteComponent,
    AddResponsableComponent,
    AddCoordinatorComponent,
    ViewInternshipsComponent,
    ViewIntershipProposalsComponent,
    AddInternshipComponent,
    ViewProjectsComponent,
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
          {
            path: "addUser",
            component: AddUserComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
          {
            path: "addDocente",
            component: AddDocenteComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
          {
            path: "addCompany",
            component: AddCompanyComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
          {
            path: "addResponsable",
            component: AddResponsableComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
          {
            path: "addCoordinator",
            component: AddCoordinatorComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin"] },
          },
          { path: "viewInternships", component: ViewInternshipsComponent },
          {
            path: "viewProposedInternship",
            component: ViewIntershipProposalsComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin", "Coordenador"] },
          },
          {
            path: "proposeInternship",
            component: AddInternshipComponent,
            canActivate: [AuthGuard],
            data: { permittedRoles: ["Admin", "ResponsavelEmpresa"] },
          },
          { path: "viewProjects", component: ViewProjectsComponent },
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
  ],
  bootstrap: [AppComponent],
})
export class AppModule { }
