import { Component, OnInit, Inject } from '@angular/core';
import { FormControl, Validators, FormGroup } from '@angular/forms';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-coordinator',
  templateUrl: './add-coordinator.component.html',
  styleUrls: ['./add-coordinator.component.css']
})
export class AddCoordinatorComponent implements OnInit {

  email = new FormControl("", [Validators.required, Validators.email]);
  public courses: Course[];
  public dataSource: MatTableDataSource<Coordinator>;
  public displayedColumns = ['name', 'number', 'course', 'email', 'delete'];

  coordinatorForm = new FormGroup({
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
    http.get<Coordinator[]>(baseUrl + 'api/Coordenators').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
      },
      error => console.error('error')
    );
  }

  createMyObject() {
    this.coordinatorForm.setValue({
      email: this.coordinatorForm.value.email,
      firstName: this.coordinatorForm.value.firstName,
      lastName: this.coordinatorForm.value.lastName,
      number: Number(this.coordinatorForm.value.number),
      CourseId: this.coordinatorForm.value.CourseId,
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
    var url = this.baseUrl + "api/Coordenators";

    this.createMyObject();
    console.log(this.coordinatorForm.value);

    this.http.post(url, this.coordinatorForm.value).subscribe(
      (res) => {
        this.toastr.success("Foi enviado um email de confirmação ao coordenador.");
        this.refreshList();
      },
      (err) => {
        this.toastr.error("Houve um erro na criação do user.");
        console.log(err);
      }
    );
  }

  deleteCoordinator(coordinatorId) {
    var url = this.baseUrl + "api/Coordenators/" + coordinatorId;
    this.http.delete(url).subscribe(
      (res) => {
        this.toastr.success("O Coordenador foi apagado.")
        this.refreshList();
      },
      (error) => {
        this.toastr.error("Houve um erro ao tentar apagar o Coordenador.");
        console.error(error);
      }
    );
  }

  refreshList() {
    this.http.get<Coordinator[]>(this.baseUrl + 'api/Coordenators').toPromise().then(
      result => this.dataSource = new MatTableDataSource(result)
    );
  }
}

interface Coordinator {
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
