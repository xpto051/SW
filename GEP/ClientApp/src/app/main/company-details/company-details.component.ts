<<<<<<< Updated upstream

import { Component, OnInit, Inject } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { MatTableDataSource } from '@angular/material';
import { HttpClient } from '@angular/common/http';
=======
import { Component, OnInit, Inject, TemplateRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material';
import { MatDialog } from '@angular/material/dialog';

>>>>>>> Stashed changes

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

<<<<<<< Updated upstream
  public displayedColumns = ['id', 'sigla', 'companyName'];
=======
  public companies: Company[];
>>>>>>> Stashed changes
  public dataSource: MatTableDataSource<Company>;
  expandedElement: Company | null;


  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string,
    public dialog: MatDialog
  ) {
    http.get<Company[]>(baseUrl + 'api/companies').subscribe(
      result => {
        this.dataSource = new MatTableDataSource(result);
        this.companies = result;
      },
      error => console.error('error')
    );
  }

  ngOnInit() {
  }

<<<<<<< Updated upstream
=======
  getDetails(id: string) {
    var comp: Company;
    console.log("fods não andas crl");

    this.companies.forEach(company => {
      if (company.id.toString() == id) {
        comp = company;
      }
    });
    let companyTemplate = new CompanyDetailsTemplateComponent(comp.id, comp.sigla, comp.companyName, comp.description, comp.url, this.dialog, this.http, this.baseUrl);
    companyTemplate.openDialogWithTemplate();
  }
>>>>>>> Stashed changes
}


interface Company {
  id: number;
  sigla: string;
  companyName: string;
  description: string;
  url: string;
<<<<<<< Updated upstream
}
=======
}


@Component({
  template: `
  <ng-template #details>
    <p>dialog-company works!</p>
    <h1 mat-dialog-title>Dialog with elements</h1>
    <div mat-dialog-content>
      This dialog showcases the title, close, content and actions elements.
      {{id}}{{sigla}}{{companyName}}{{description}}{{url}}
    </div>

    <div mat-dialog-actions>
      <button mat-button mat-dialog-close>Close</button>
    </div>
  </ng-template>
`})

export class CompanyDetailsTemplateComponent implements OnInit {

  id: any;
  sigla: any;
  companyName: any;
  description: any;
  url: any;

  constructor(id: number, sigla: string, companyName: string, description: string, url: string, private dialog: MatDialog, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string,) {
    this.id = id;
    this.sigla = sigla;
    this.companyName = companyName;
    this.description = description;
    url ? this.url = url : this.url = '';
  }

  ngOnInit() {
  }

  openDialogWithTemplate() {
    this.dialog.open(CompanyDetailsTemplateComponent);
  }
}
>>>>>>> Stashed changes
