import { Injectable } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {IEmployee} from "../interfaces/iemployee";
import {Observable} from "rxjs";
import {basePath} from "../constants/server";
import {api} from "../constants/api";

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {

  constructor(private http: HttpClient) { }

  getEmployees() : Observable<IEmployee[]> {
    return this.http.get<IEmployee[]>(basePath + api.employees);
  }
}
