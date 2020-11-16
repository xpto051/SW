import { Component, OnInit, Inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-view-intership-proposals',
  templateUrl: './view-intership-proposals.component.html',
  styleUrls: ['./view-intership-proposals.component.css'],
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
export class ViewIntershipProposalsComponent implements OnInit {

  public displayedColumns = ['role', 'companyName', 'vagas'];
  public dataSource: MatTableDataSource<Internship>;
  expandedElement: Company | null;


  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    private toastr: ToastrService
  ) {
    http.get<Internship[]>(baseUrl + 'api/Internships/proposeInternship').subscribe(
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

    var url = this.baseUrl + "api/Internships/acceptPropose/" + proposalId;
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

    var url = this.baseUrl + "api/Internships/rejectPropose/" + proposalId;
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
    this.http.get<Internship[]>(this.baseUrl + 'api/Internships/proposeInternship').toPromise().then(
      result => this.dataSource = new MatTableDataSource(result)
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