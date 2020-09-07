import { Component, OnInit, Inject } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-view-internships',
  templateUrl: './view-internships.component.html',
  styleUrls: ['./view-internships.component.css'],
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
export class ViewInternshipsComponent implements OnInit {

  public displayedColumns = ['role', 'companyName', 'vagas'];
  public dataSource: MatTableDataSource<Internship>;
  expandedElement: Company | null;


  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    http.get<Internship[]>(baseUrl + 'api/Internships/availableInternships').subscribe(
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