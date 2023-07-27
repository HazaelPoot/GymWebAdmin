import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ModalService {

  private valorSubject: Subject<number> = new Subject<number>();

  enviarValor(valor: number): void {
    this.valorSubject.next(valor);
  }

  getValor(): Subject<number> {
    return this.valorSubject;
  }
}