import { ResponseApi } from '../Interfaces/response.interface';
import { environment } from 'src/environments/environment';
import { Usuario } from '../Interfaces/usuario.interface';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberService {

  private url: string = environment.endpoint + 'Miembro';

  constructor(private http: HttpClient) { }

  listaMiembro(idGym: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/List/${idGym}`)
  }

  createMiembro(idGym: number, request: Usuario){
    return this.http.post<ResponseApi>(`${this.url}/Agregar/${idGym}`, request);
  }

  createMembresia(idG: number, idU: number, ){
    return this.http.get<ResponseApi>(`${this.url}/Membresia/${idG}/${idU}`);
  }

  deleteMiembro(idMiembro: number){
    return this.http.delete<ResponseApi>(`${this.url}/Delete/${idMiembro}`);
  }
}