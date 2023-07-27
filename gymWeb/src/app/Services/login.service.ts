import { ResponseApi } from '../Interfaces/response.interface';
import { environment } from 'src/environments/environment';
import { Login } from '../Interfaces/login.interface';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  private url: string = environment.endpoint + 'Acceso'

  constructor(private http: HttpClient) { }

  login(credentials: Login): Observable<ResponseApi>{
    return this.http.post<ResponseApi>(this.url, credentials);
  }
}