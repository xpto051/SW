import { Component, OnInit, Inject } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';
import { trigger, state, style, transition, animate } from '@angular/animations';

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
    @Inject('BASE_URL') private baseUrl: string
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