import { ResponseApi } from '../Interfaces/response.interface';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private url: string = environment.endpoint + 'Horario';

  constructor(private http: HttpClient) { }

  listaHorario(idGym: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/List/${idGym}`)
  }

  getById(id: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/GetById/${id}`)
  }

  createHorario(idGym: number, rerquest: FormData): Observable<ResponseApi>{
    return this.http.post<ResponseApi>(`${this.url}/Create/${idGym}`, rerquest);
  }

  updateHorario(rerquest: FormData): Observable<ResponseApi>{
    return this.http.put<ResponseApi>(`${this.url}/Update/`, rerquest);
  }

  deleteHorario(id: number): Observable<ResponseApi> {
    return this.http.delete<ResponseApi>(`${this.url}/Delete/${id}`);
  }
}
