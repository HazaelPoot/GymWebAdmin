import { ValidatormemberComponent } from './Components/layout/Pages/validatormember/validatormember.component';
import { DeleteaccountComponent } from './Components/layout/Pages/deleteaccount/deleteaccount.component';
import { SubscriptionsComponent } from './Components/layout/Pages/subscriptions/subscriptions.component';
import { InformationComponent } from './Components/layout/Pages/information/information.component';
import { GymservicesComponent } from './Components/layout/Pages/gymservices/gymservices.component';
import { ChangepassComponent } from './Components/layout/Pages/changepass/changepass.component';
import { DashboardComponent } from './Components/layout/Pages/dashboard/dashboard.component';
import { ScheduleComponent } from './Components/layout/Pages/schedule/schedule.component';
import { MembersComponent } from './Components/layout/Pages/members/members.component';
import { RegisterComponent } from './Components/register/register.component';
import { LayoutComponent } from './Components/layout/layout.component';
import { LoginComponent } from './Components/login/login.component';
import { RouterModule, Routes } from '@angular/router';
import { NgModule } from '@angular/core';

const routes: Routes = [
  { path: '', component: LoginComponent, pathMatch: 'full' },
  { path: 'Login', component: LoginComponent, pathMatch: 'full' },
  { path: 'Register', component: RegisterComponent, pathMatch: 'full' },
  {
    path: 'Pages',
    component: LayoutComponent,
    children: [
      { path: 'Dashboard', component: DashboardComponent },
      { path: 'Information', component: InformationComponent },
      { path: 'Services', component: GymservicesComponent },
      { path: 'Schedules', component: ScheduleComponent },
      { path: 'Members', component: MembersComponent },
      { path: 'Subscriptions', component: SubscriptionsComponent },
      { path: 'Validator', component: ValidatormemberComponent },
      { path: 'ChangePass', component: ChangepassComponent },
      { path: 'DeleteAccount', component: DeleteaccountComponent },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }