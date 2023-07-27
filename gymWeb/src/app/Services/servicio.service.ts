import { ResponseApi } from '../Interfaces/response.interface';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServicioService {

  private url: string = environment.endpoint + 'Servicio';

  constructor(private http: HttpClient) { }

  listaServicio(idGym: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/List/${idGym}`)
  }

  getById(id: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/GetById/${id}`)
  }

  createServicio(idGym: number, request: FormData): Observable<ResponseApi>{
    return this.http.post<ResponseApi>(`${this.url}/Create/${idGym}`, request);
  }

  updateServicio(request: FormData): Observable<ResponseApi>{
    return this.http.put<ResponseApi>(`${this.url}/Update/`, request);
  }

  deleteServicio(id: number): Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.url}/Delete/${id}`);
  }
}
