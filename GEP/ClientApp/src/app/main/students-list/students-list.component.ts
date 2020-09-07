import { Component, OnInit } from "@angular/core";
import { PerfectScrollbarConfigInterface } from "ngx-perfect-scrollbar";

@Component({
  selector: "app-students-list",
  templateUrl: "./students-list.component.html",
  styleUrls: ["./students-list.component.css"],
})
export class StudentsListComponent implements OnInit {
  public config: PerfectScrollbarConfigInterface = {};

  public disabled: boolean = true;

  constructor() {}

  public toggleDisabled(): void {
    this.disabled = !this.disabled;
  }

  ngOnInit() {}
}
