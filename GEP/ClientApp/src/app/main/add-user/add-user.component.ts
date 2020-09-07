import { Component, OnInit, Inject } from "@angular/core";
import { FormControl, Validators, NgForm, FormGroup } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { ToastrService } from "ngx-toastr";
import { MatTableDataSource } from "@angular/material";

@Component({
  selector: "app-add-user",
  templateUrl: "./add-user.component.html",
  styleUrls: ["./add-user.component.css"],
})
export class AddUserComponent implements OnInit {
  email = new FormControl("", [Validators.required, Validators.email]);
  public courses: Course[];
  public dataSource: MatTableDataSource<Student>;
  public displayedColumns = ['name', 'number', 'course', 'emailConfirmed', 'delete'];

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
  ) {
    http.get<Student[]>(baseUrl + 'api/students').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
      },
      error => console.error('error')
    );
  }

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
        this.refreshList();
      },
      (err) => {
        this.toastr.error("Houve um erro na criação do user");
        console.log(err);
      }
    );
  }

  deleteAluno(studentId) {
    var url = this.baseUrl + "api/students/" + studentId;
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
    this.http.get<Student[]>(this.baseUrl + 'api/students').toPromise().then(
      result => this.dataSource = new MatTableDataSource(result)
    );
  }
}

interface Student {
  id: number;
  number: number;
  courseId: number;
  course: Course;
  userId: string;
  user: User;
}

interface Course {
  id: number;
  sigla: string;
  designação: string;
}

interface User {
  name: string;
  email: string;
  emailConfirmed: boolean;
}