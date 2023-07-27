import { SpinnerComponent } from 'src/app/Components/spinner/spinner.component';
import { HttpClientModule} from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';

@NgModule({
  declarations: [
    SpinnerComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    FormsModule,
    //NO SE IMPORTA LAYOUT, PORQUE ESTE MODULO SE USA EN LAYOUT Y OCURRIRIA UNA RECURSIVIDAD.
  ],
  exports: [
    //TODOS LOS MODULOS SE PONEN AQUI, PARA QUE SEA UN MODULO EL QUE EXPORTE TODOS.
    SpinnerComponent,
  ]
})
export class SharedModule { }