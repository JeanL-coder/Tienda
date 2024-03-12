import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup, FormControl } from '@angular/forms';
import ValidateForm from '../../helpers/validateForms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
type: string ="password";
isText: boolean = false;
eyeIcon: string = "fa-eye-slash";
public loginForm!: FormGroup;
constructor(
  private fb: FormBuilder, 
  private auth: AuthService, 
  private router : Router,
  private toastr: ToastrService)
  {}

ngOnInit(): void {
  this.loginForm = this.fb.group({
  Correo:["", Validators.required],
  Password:["", Validators.required],
  })
}

  hideShowPass(){
   this.isText = !this.isText;
   this.isText ? this.eyeIcon = "fa-eye" : this.eyeIcon = "fa-eye-slash";
   this.isText ? this.type = "text" : this.type = "password";
  }
  onSubmit() {
    if (this.loginForm.valid) {
      console.log(this.loginForm.value)
      this.auth.login(this.loginForm.value).subscribe({
        next: (res) => {
          console.log(res.message);
          this.loginForm.reset();
          this.auth.storeToken(res.token);
          this.toastr.success(res.message, 'Login')
          this.router.navigate(["home"]);
        },
        error: (err) => {
          if (err.status === 400) {
            const errorMessage = JSON.stringify(err.error);
            this.toastr.error(err.error.Message, 'Error de validación');
          } else if (err.status === 404) {
            this.toastr.error(err.error.Message, 'Usuario no encontrado');
          } else {
            this.toastr.error('Error inesperado', 'Error');
          }
          console.log(err);
        }
      });
    } else {
      ValidateForm.validateFields(this.loginForm);
      this.toastr.warning('Formulario Inválido', 'Advertencia');
    }
  }
}
