import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-company',
  templateUrl: './add-company.component.html',
  styleUrls: ['./add-company.component.css']
})
export class AddCompanyComponent implements OnInit {

  companyForm = new FormGroup({
    sigla: new FormControl(""),
    companyName: new FormControl(""),
    description: new FormControl(""),
    url: new FormControl(""),
  });

  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) { }

  createMyObject() {
    this.companyForm.setValue({
      sigla: this.companyForm.value.sigla,
      companyName: this.companyForm.value.companyName,
      description: this.companyForm.value.description,
      url: this.companyForm.value.url,
    });
  }

  ngOnInit() {
  }

  onSubmit() {
    var url = this.baseUrl + "api/companies";

    this.createMyObject();
    console.log(this.companyForm.value);

    this.http.post(url, this.companyForm.value).subscribe(
      (res) => {
        this.toastr.success("A empresa foi adicionada com sucesso.");
      },
      (err) => {
        this.toastr.error("Houve um erro na criação da empresa.");
        console.log(err);
      }
    );
  }

}


interface Company {
  sigla: string;
  companyName: string;
  description: string;
  url: string;
}