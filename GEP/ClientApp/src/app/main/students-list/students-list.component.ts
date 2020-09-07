import { Component, OnInit, Inject } from "@angular/core";
import { PerfectScrollbarConfigInterface } from "ngx-perfect-scrollbar";
import { HttpClient } from "@angular/common/http";
import { MatTableDataSource } from "@angular/material";
import { trigger, state, transition, style, animate } from "@angular/animations";

@Component({
  selector: "app-students-list",
  templateUrl: "./students-list.component.html",
  styleUrls: ["./students-list.component.css"],
  animations: [
    trigger("detailExpand", [
      state("collapsed", style({ height: "0px", minHeight: "0" })),
      state("expanded", style({ height: "*" })),
      transition(
        "expanded <=> collapsed",
        animate("225ms cubic-bezier(0.4, 0.0, 0.2, 1)")
      )
    ])
  ]
})
export class StudentsListComponent implements OnInit {
  public config: PerfectScrollbarConfigInterface = {};

  public dataSource: MatTableDataSource<Student>;
  public displayedColumns = ['name', 'number', 'course'];
  expandedElement: Student | null;


  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
  ) {
    http.get<Student[]>(baseUrl + 'api/students').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
        console.log(result);
      },
      error => console.error('error')
    );
  }


  ngOnInit() {
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
}
