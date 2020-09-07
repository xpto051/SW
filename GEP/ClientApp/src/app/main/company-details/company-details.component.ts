
import { Component, OnInit, Inject } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-company-details',
  templateUrl: './company-details.component.html',
  styleUrls: ['./company-details.component.css'],
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

export class CompanyDetailsComponent implements OnInit {

  public displayedColumns = ['id', 'sigla', 'companyName'];
  public dataSource: MatTableDataSource<Company>;
  expandedElement: Company | null;

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {
    http.get<Company[]>(baseUrl + 'api/companies').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
      },
      error => console.error('error')
    );
  }

  ngOnInit() {
  }

}


interface Company {
  id: number;
  sigla: string;
  companyName: string;
  description: string;
  url: string;
}