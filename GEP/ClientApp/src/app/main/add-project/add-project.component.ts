import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.css']
})
export class AddProjectComponent implements OnInit {

  projectForm = new FormGroup({
    theme: new FormControl(""),
    vagas: new FormControl(""),
    description: new FormControl(""),
  });


  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) { }

  createMyObject() {
    this.projectForm.setValue({
      theme: this.projectForm.value.theme,
      vagas: Number(this.projectForm.value.vagas),
      description: this.projectForm.value.description,
    });
  }

  ngOnInit() {
  }

  onSubmit() {
    var url = this.baseUrl + "api/Projects/proposeProject";

    this.createMyObject();
    console.log(this.projectForm.value);

    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });
    console.log(token);

    this.http.post(url, this.projectForm.value, { headers: token }).subscribe(
      (res) => {
        this.toastr.success("O projeto foi proposto com sucesso.");
      },
      (err) => {
        this.toastr.error("Houve um erro na criação da proposta de projeto.");
        console.log(err);
      }
    );
  }


}

interface Project {
  id: number;
  theme: string;
  professorId: number;
  professor: Professor;
  vagas: number;
  proposta: boolean;
  aceite: boolean;
  description: string;
}

interface Professor {
  id: number;
  number: string;
  userId: number;
  user: User;
}

interface User {
  name: string;
  email: string;
  emailConfirmed: boolean;
}