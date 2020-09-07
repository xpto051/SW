import { Component, OnInit, Inject } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { MatTableDataSource } from '@angular/material';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-view-project-proposals',
  templateUrl: './view-project-proposals.component.html',
  styleUrls: ['./view-project-proposals.component.css'],
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
export class ViewProjectProposalsComponent implements OnInit {

  public displayedColumns = ['theme', 'vagas'];
  public dataSource: MatTableDataSource<Project>;
  expandedElement: Project | null;


  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private toastr: ToastrService
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

  acceptProposal(proposalId) {
    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });

    var url = this.baseUrl + "api/Projects/acceptPropose/" + proposalId;
    this.http.put(url, { headers: token }).subscribe(
      (res) => {
        this.toastr.success("A proposta foi aceite.");
        this.refreshList();
      },
      (error) => {
        this.toastr.error("Ocurreu um erro ao tentar aceitar a proposta.");
        console.error(error);
      }
    );
  }

  rejectProposal(proposalId) {
    var token = new HttpHeaders({
      Authorization: "Bearer " + localStorage.getItem("token"),
    });

    var url = this.baseUrl + "api/Projects/rejectPropose/" + proposalId;
    this.http.put(url, { headers: token }).subscribe(
      (res) => {
        this.toastr.success("A proposta foi rejeitada.");
        this.refreshList();
      },
      (error) => {
        this.toastr.error("Ocurreu um erro ao tentar rejeitar a proposta.");
        console.error(error);
      }
    );
  }

  refreshList() {
    this.http.get<Project[]>(this.baseUrl + 'api/Projects/proposeProjects').toPromise().then(
      result => this.dataSource = new MatTableDataSource(result)
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