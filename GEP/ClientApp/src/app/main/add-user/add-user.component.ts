import { Component, OnInit, Inject } from "@angular/core";
import { FormControl, Validators, NgForm, FormGroup } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";

@Component({
  selector: "app-add-user",
  templateUrl: "./add-user.component.html",
  styleUrls: ["./add-user.component.css"],
})
export class AddUserComponent implements OnInit {
  email = new FormControl("", [Validators.required, Validators.email]);
  public courses: Course[];

  estudanteForm = new FormGroup({
    email: new FormControl(""),
    firstName: new FormControl(""),
    lastName: new FormControl(""),
    number: new FormControl(""),
    CourseId: new FormControl(""),
  });

  constructor(
    private http: HttpClient,
    @Inject("BASE_URL") private baseUrl: string,
    private toastr: ToastrService
  ) { }

  createMyObject() {
    this.estudanteForm.setValue({
      email: this.estudanteForm.value.email,
      firstName: this.estudanteForm.value.firstName,
      lastName: this.estudanteForm.value.lastName,
      number: Number(this.estudanteForm.value.number),
      CourseId: this.estudanteForm.value.CourseId,
    });
  }

  ngOnInit() {
    this.getCourses();
  }

  getErrorMessage() {
    if (this.email.hasError("required")) {
      return "Este campo é obrigatório";
    }

    return this.email.hasError("email") ? "Not a valid email" : "";
  }

  getCourses() {
    var url = this.baseUrl + "api/courses";
    this.http.get<Course[]>(url).subscribe(
      (res) => {
        this.courses = res;
      },
      (error) => console.error(error)
    );
  }
  onSubmit() {
    var url = this.baseUrl + "api/students";

    this.createMyObject();
    console.log(this.estudanteForm.value);

    this.http.post(url, this.estudanteForm.value).subscribe(
      (res) => {
        this.toastr.success("Foi enviado um email de confirmação ao estudante");
      },
      (err) => {
        this.toastr.error("Houve um erro na criação do user");
        console.log(err);
      }
    );
  }
}

interface Course {
  id: number;
  sigla: string;
  designacao: string;
}
