import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatTableDataSource } from '@angular/material';
import { DialogCompanyComponent } from '../components/dialog-company/dialog-company.component';

@Component({
  selector: 'app-company-details',
  templateUrl: './company-details.component.html',
  styleUrls: ['./company-details.component.css']
})
export class CompanyDetailsComponent implements OnInit {

  public dataSource: MatTableDataSource<Company>;
  public displayedColumns = ['id', 'sigla', 'companyName', 'details'];

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

  getDetails() {
    console.log("fods não andas crl");
  }


}

interface Company {
  id: number;
  sigla: string;
  companyName: string;
  description: string;
  url: string;
}

