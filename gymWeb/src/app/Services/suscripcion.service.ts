import { Susucripcion } from '../Interfaces/suscripcion.interface';
import { ResponseApi } from '../Interfaces/response.interface';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SuscripcionService {

  private url: string = environment.endpoint + 'Suscripcion';

  constructor(private http: HttpClient) { }

  listSuscription(idGym: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/List/${idGym}`)
  }

  searchMember(idGym: number, value: string): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/Search/${idGym}/${value}`)
  }

  registerSuscription(request: Susucripcion): Observable<ResponseApi>{
    return this.http.post<ResponseApi>(`${this.url}/Register`, request);
  }

  changePay(request: Susucripcion): Observable<ResponseApi>{
    return this.http.put<ResponseApi>(`${this.url}/ChangePay`, request);
  }

  deleteSuscription(id: number): Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.url}/Delete/${id}`);
  }
}