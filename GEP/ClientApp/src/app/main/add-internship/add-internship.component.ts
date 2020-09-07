import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-internship',
  templateUrl: './add-internship.component.html',
  styleUrls: ['./add-internship.component.css']
})
export class AddInternshipComponent implements OnInit {

  internshipForm = new FormGroup({
    role: new FormControl(""),
    vagas: new FormControl(""),
    description: new FormControl(""),
  });


  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) { }

  createMyObject() {
    this.internshipForm.setValue({
      role: this.internshipForm.value.role,
      vagas: Number(this.internshipForm.value.vagas),
      description: this.internshipForm.value.description,
    });
  }

  ngOnInit() {
  }

  onSubmit() {
    var url = this.baseUrl + "api/Internships/proposeInternship";

    this.createMyObject();
    console.log(this.internshipForm.value);

    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });
    console.log(token);

    this.http.post(url, this.internshipForm.value, { headers: token }).subscribe(
      (res) => {
        this.toastr.success("O estágio foi proposto com sucesso.");
      },
      (err) => {
        this.toastr.error("Houve um erro na criação da proposta de estágio.");
        console.log(err);
      }
    );
  }


}

interface Internship {
  id: number;
  role: string;
  companyId: number;
  company: Company;
  companyRespId: number;
  companyResp: CompanyResponsable;
  vagas: number;
  proposta: boolean;
  aceite: boolean;
  description: string;
}

interface Company {
  id: number;
  sigla: string;
  companyName: string;
  description: string;
  url: string;
}

interface CompanyResponsable {
  email: string;
  name: string;
  company: Company;
}