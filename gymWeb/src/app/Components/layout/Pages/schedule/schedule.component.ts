import { ScheduleService } from 'src/app/Services/schedule.service';
import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { Horario } from 'src/app/Interfaces/horario.interface';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent implements OnInit {
  horarios: Horario[] = [];
  public idGym: number;

  constructor(
    private scheduleService: ScheduleService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit() {
    this.obtenerHorarios();
  }

  obtenerHorarios() {
    this.loadingService.setLoading(true);
    this.scheduleService.listaHorario(this.idGym).subscribe({
      next: (res) => {
        if (res.status == true) {
          this.horarios = res.listObject;
          this.loadingService.setLoading(false);
        } else {
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      },
    });
  }

  enviarValor(idHorario: number): void {
    this.modalService.enviarValor(idHorario);
  }

  btnDelete(idHorario: number) {
    Swal.fire({
      icon: 'question',
      title: 'Confirmación',
      text: `¿Estás seguro de eliminar este horario?`,
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteService(idHorario);
      }
    });
  }

  deleteService(idHorario: number) {
    this.loadingService.setLoading(true);
    this.scheduleService.deleteHorario(idHorario).subscribe({
      next: (res) => {
        if (res.status == true) {
          this._swal.swalSucces(res.message);
        } else if (res.status == false) {
          this._swal.swalOops(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      },
    });
  }
}
