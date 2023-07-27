import { ModalsuscripcionComponent } from './modalsuscripcion/modalsuscripcion.component';
import { ModalservicioComponent } from './modalservicio/modalservicio.component';
import { ModalhorarioComponent } from './modalhorario/modalhorario.component';
import { ModalmiembroComponent } from './modalmiembro/modalmiembro.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [
    ModalservicioComponent,
    ModalhorarioComponent,
    ModalmiembroComponent,
    ModalsuscripcionComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
  ],
  exports: [
    ModalservicioComponent,
    ModalhorarioComponent,
    ModalmiembroComponent,
    ModalsuscripcionComponent,
  ]
})
export class ModalesModule { }
