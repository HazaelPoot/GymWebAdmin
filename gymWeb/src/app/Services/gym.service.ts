import { ResponseApi } from '../Interfaces/response.interface';
import { Password } from '../Interfaces/password.interface';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GymService {

  private url: string = environment.endpoint + 'Gym';

  constructor(private http: HttpClient) { }

  obtainGym(idGimansio: number): Observable<ResponseApi>{
    return this.http.get<ResponseApi>(`${this.url}/${idGimansio}`);
  }

  registerGym(request: FormData): Observable<ResponseApi>{
    return this.http.post<ResponseApi>(`${this.url}/Create`, request);
  }

  guardarPerfil(rerquest: FormData): Observable<ResponseApi>{
    return this.http.put<ResponseApi>(`${this.url}/SaveProfile`, rerquest);
  }

  changePass(id: number, dtoPass: Password): Observable<ResponseApi>{
    return this.http.put<ResponseApi>(`${this.url}/ChangePass/${id}`, dtoPass);
  }

  deleteAccount(id: number, dtoPass: Password): Observable<ResponseApi> {
    return this.http.post<ResponseApi>(`${this.url}/DeleteAccount/${id}`, dtoPass);
  }
}