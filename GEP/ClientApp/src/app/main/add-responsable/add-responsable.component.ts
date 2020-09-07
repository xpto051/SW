import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-responsable',
  templateUrl: './add-responsable.component.html',
  styleUrls: ['./add-responsable.component.css']
})
export class AddResponsableComponent implements OnInit {

  email = new FormControl("", [Validators.required, Validators.email]);

  public companies: Company[];

  public dataSource: MatTableDataSource<CompanyResponsable>;
  public displayedColumns = ['name', 'email', 'companyName', 'delete'];

  responsableForm = new FormGroup({
    email: new FormControl(""),
    firstName: new FormControl(""),
    lastName: new FormControl(""),
    CompanyId: new FormControl(""),
  });


  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) {
    http.get<CompanyResponsable[]>(baseUrl + 'api/CompanyResps').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
      },
      error => console.error('error')
    );
  }

  createMyObject() {
    this.responsableForm.setValue({
      email: this.responsableForm.value.email,
      firstName: this.responsableForm.value.firstName,
      lastName: this.responsableForm.value.lastName,
      CompanyId: this.responsableForm.value.CompanyId,
    });
  }

  ngOnInit() {
    this.getEmpresas();
  }

  getErrorMessage() {
    if (this.email.hasError("required")) {
      return "Este campo é obrigatório";
    }

    return this.email.hasError("email") ? "Not a valid email" : "";
  }

  getEmpresas() {
    var url = this.baseUrl + "api/companies";
    this.http.get<Company[]>(url).subscribe(
      (res) => {
        this.companies = res;
      },
      (error) => console.error(error)
    );
  }

  onSubmit() {
    var url = this.baseUrl + "api/CompanyResps";

    this.createMyObject();
    console.log(this.responsableForm.value);

    this.http.post(url, this.responsableForm.value).subscribe(
      (res) => {
        this.toastr.success("Foi enviado um email de confirmação ao responsável.");
        this.refreshList();
      },
      (err) => {
        this.toastr.error("Houve um erro na criação do user");
        console.log(err);
      }
    );
  }

  deleteResponsable(responsableId) {
    var url = this.baseUrl + "api/CompanyResps/" + responsableId;
    this.http.delete(url).subscribe(
      (res) => {
        this.toastr.success("O Responsável da Empresa foi apagado.")
        this.refreshList();
      },
      (error) => {
        this.toastr.error("Houve um erro ao tentar apagar o Responsável da Empresa.");
        console.error(error);
      }
    );
  }

  refreshList() {
    this.http.get<CompanyResponsable[]>(this.baseUrl + 'api/CompanyResps').toPromise().then(
      result => this.dataSource = new MatTableDataSource(result)
    );
  }

}

interface CompanyResponsable {
  email: string;
  name: string;
  company: Company;
}

interface Company {
  id: number;
  sigla: string;
  companyName: string;
}