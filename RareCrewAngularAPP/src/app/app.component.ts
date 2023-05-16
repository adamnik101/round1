import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import {EmployeeService} from "./services/employee.service";
import {IEmployee} from "./interfaces/iemployee";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {


  constructor( ) {

  }

  title = 'RareCrewAngularAPP';
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
