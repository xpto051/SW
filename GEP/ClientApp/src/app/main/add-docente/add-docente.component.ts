import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-docente',
  templateUrl: './add-docente.component.html',
  styleUrls: ['./add-docente.component.css']
})
export class AddDocenteComponent implements OnInit {

  email = new FormControl("", [Validators.required, Validators.email]);
  public dataSource: MatTableDataSource<Docente>;
  public displayedColumns = ['name', 'email', 'number', 'delete'];

  docenteForm = new FormGroup({
    email: new FormControl(""),
    firstName: new FormControl(""),
    lastName: new FormControl(""),
    number: new FormControl(""),
  });


  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) {
    http.get<Docente[]>(baseUrl + 'api/professors').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
      },
      error => console.error('error')
    );
  }

  createMyObject() {
    this.docenteForm.setValue({
      email: this.docenteForm.value.email,
      firstName: this.docenteForm.value.firstName,
      lastName: this.docenteForm.value.lastName,
      number: Number(this.docenteForm.value.number),
    });
  }

  ngOnInit() {
  }

  getErrorMessage() {
    if (this.email.hasError("required")) {
      return "Este campo é obrigatório";
    }

    return this.email.hasError("email") ? "Not a valid email" : "";
  }

  onSubmit() {
    var url = this.baseUrl + "api/students";

    this.createMyObject();
    console.log(this.docenteForm.value);

    this.http.post(url, this.docenteForm.value).subscribe(
      (res) => {
        this.toastr.success("Foi enviado um email de confirmação ao docente.");
        this.refreshList();
      },
      (err) => {
        this.toastr.error("Houve um erro na criação do user");
        console.log(err);
      }
    );
  }

  deleteDocente(docenteId) {
    var url = this.baseUrl + "api/professors/" + docenteId;
    this.http.delete(url).subscribe(
      (res) => {
        this.toastr.success("O Aluno foi apagado.")
        this.refreshList();
      },
      (error) => {
        this.toastr.error("Houve um erro a tentar apagar o Aluno.");
        console.error(error);
      }
    );
  }

  refreshList() {
    this.http.get<Docente[]>(this.baseUrl + 'api/professors').toPromise().then(
      result => this.dataSource = new MatTableDataSource(result)
    );
  }

}

interface Docente {
  email: string;
  name: string;
  number: number;
}