import { ScheduleService } from 'src/app/Services/schedule.service';
import { SessionService } from 'src/app/Services/session.service';
import { LoaderService } from 'src/app/Services/loader.service';
import { Horario } from 'src/app/Interfaces/horario.interface';
import { ModalService } from 'src/app/Services/modal.service';
import { SwalService } from 'src/app/Services/swal.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-modalhorario',
  templateUrl: './modalhorario.component.html',
  styleUrls: ['./modalhorario.component.scss'],
})
export class ModalhorarioComponent implements OnInit {

  public idHorario: number = 0;
  public add: boolean = true;
  public idGym: number;

  public horarioModel: Horario = {
    idHorario: 0,
    diaSemana: '',
    horaInicio: '',
    horaFin: '',
    idGimnasio: 0,
    nombreGimnasio: '',
  };

  constructor(
    private horarioService: ScheduleService,
    private sessionService: SessionService,
    private loadingService: LoaderService,
    private modalService: ModalService,
    private _swal: SwalService,
  ) {
    this.idGym = this.sessionService.obtenerSession();
  }

  ngOnInit() {
    this.obtainIdHorario();
  }

  obtainIdHorario(): void {
    this.modalService.getValor().subscribe((valor: number) => {
      this.idHorario = valor;
      this.setModel();
    });
  }

  setModel(){
    this.horarioService.getById(this.idHorario).subscribe({
      next: (res) =>{
        this.resetearHorarioModel();
        if(res.status == true){
          this.horarioModel = res.object;
          this.add = false;
        }
        else{
          this.add = true;
        }
      }
    });
  }

  //METODO POST (AGREGAR)
  btnGuardar(){
    if (this.horarioModel.diaSemana == '' || this.horarioModel.horaInicio == '' || this.horarioModel.horaFin == '') {
      this._swal.swalValidator();
    }
    else{
      this.createHorario();
    }
  }

  createHorario(){
    this.loadingService.setLoading(true);
    const horarioModelModelo: string = JSON.stringify(this.horarioModel);
    const formData = new FormData();

    formData.append('modelo', horarioModelModelo);

    this.horarioService.createHorario(this.idGym, formData).subscribe({
      next: (res) => {
        if(res.status == true){
          this._swal.swalSucces(res.message);
        }
        else if(res.status == false){
          this._swal.swalOops(res.message);
        }
      },
      error: (err) => {
        this._swal.swalError();
      }
    });
  }

  //METODO PUT (ACTUALIZAR)
  btnActualizar(){
    if (this.horarioModel.diaSemana == '' || this.horarioModel.horaInicio == '' || this.horarioModel.horaFin == '') {
      this._swal.swalValidator();
    }
    else{
      this.updateHorario();
    }
  }

  updateHorario(){
    this.loadingService.setLoading(true);
    const horarioModelModelo: string = JSON.stringify(this.horarioModel);
    const formData = new FormData();

    formData.append('modelo', horarioModelModelo);

    this.horarioService.updateHorario(formData).subscribe({
      next: (res) => {
        if(res.status == true){
          this._swal.swalSucces(res.message);
        }
        else if(res.status == false){
          this._swal.swalSucces(res.message);
        }
      },
      error: () => {
        this._swal.swalError();
      }
    });
  }

  resetearHorarioModel(): void {
    this.horarioModel = {
      idHorario: 0,
      diaSemana: '',
      horaInicio: '',
      horaFin: '',
      idGimnasio: 0,
      nombreGimnasio: '',
    }
  }
}
