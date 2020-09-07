import { Component, OnInit, Inject } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-view-projects',
  templateUrl: './view-projects.component.html',
  styleUrls: ['./view-projects.component.css'],
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
export class ViewProjectsComponent implements OnInit {

  public displayedColumns = ['theme', 'vagas'];
  public dataSource: MatTableDataSource<Project>;
  expandedElement: Project | null;


  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    http.get<Project[]>(baseUrl + 'api/Projects/proposeProjects').subscribe(
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