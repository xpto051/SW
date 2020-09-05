import { Component, OnInit, Inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-dialog-company',
  templateUrl: './dialog-company.component.html',
  styleUrls: ['./dialog-company.component.css']
})
export class DialogCompanyComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit() {

  }

  openDialog() {
    this.dialog.open(DialogElementsExampleDialog);
  }

}

/*@Component({
  selector: 'dialog-elements-example-dialog',
  templateUrl: 'dialog-elements-example-dialog.html',
})*/
export class DialogElementsExampleDialog { }
