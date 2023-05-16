import { Component } from '@angular/core';
import {IEmployee} from "../interfaces/iemployee";
import {EmployeeService} from "../services/employee.service";

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})
export class EmployeeComponent {
  employees! :IEmployee[];

  constructor( private employeeService: EmployeeService) {

  }
  ngOnInit() :void {
    this.employeeService.getEmployees().subscribe(data => this.employees = data);
  }
}
