import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { MemberService } from 'src/app/Services/member.service';
import { Miembro } from 'src/app/Interfaces/miembro.interface';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-members',
  templateUrl: './members.component.html',
  styleUrls: ['./members.component.scss']
})
export class MembersComponent implements OnInit {

  miembros: Miembro[] = [];
  public idGym: number;

  constructor(
    private memberService: MemberService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit(){
    this.obtenerMiembros();
  }

  obtenerMiembros(){
    this.loadingService.setLoading(true);
    this.memberService.listaMiembro(this.idGym).subscribe({
      next: (res) => {
        if (res.status == true) {
          this.miembros = res.listObject;
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

  btnDelete(idMiembro: number) {
    Swal.fire({
      icon: 'question',
      title: 'Confirmación',
      text: `¿Estás seguro de eliminar esta membresia?`,
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No',
    }).then((result) => {
      if (result.isConfirmed) {
        this.deleteService(idMiembro);
      }
    });
  }

  deleteService(idMiembro: number) {
    this.loadingService.setLoading(true);
    this.memberService.deleteMiembro(idMiembro).subscribe({
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
