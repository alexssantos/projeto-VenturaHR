import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { tap } from 'rxjs/operators'
import { SessionManagerService } from './session-mng.service';
import { Router } from '@angular/router';

@Injectable({
	providedIn: 'root'
})
export class SignInUpService {
	private API_URL = environment.API_URL;

	constructor(
		private http: HttpClient,
		private toastr: ToastrService,
		private sessionService: SessionManagerService,
		private router: Router
	) { }

	public SignIn(email, senha): Observable<any> {
		let startRequestToast = this.toastr.info("Login iniciado", "VenturaHR");

		const body = {
			login: email,
			senha: senha
		}
		let url = this.API_URL+ "/auth/login";
		return this.http.post(url, body)
			.pipe(
				tap((res) => {
						this.toastr.clear(startRequestToast.toastId)
						this.toastr.success("Login finalizado com secesso", "VenturaHR");
						
						this.sessionService.setToken(res.token);
						this.sessionService.setTypeUser(res.tipoUsuario);
					},
					(error: HttpErrorResponse) => {
						this.toastr.clear(startRequestToast.toastId)
						
						if (error.status == 404) {
							let bodyError = error.error;
							this.toastr.error(bodyError.message, "VenturaHR");	
						}				
						else{
							this.toastr.error("Erro inesperado", "VenturaHR");
						}
					}),
			)
	}

	public SignUp(form: any): Observable<any> {
		
		const body = {
			nome: form.name,
			login: form.login,
			senha: form.password,
			email: form.email,
			dataNascimento: form.birthDate,
			tipo: form.typeUser,
			documento: form.document
		}

		let url = this.API_URL+ "/usuario/cadastro";
		return this.http.post(url, body)
			.pipe(
				tap((res) => {		
						this.toastr.success("Cadastro finalizado com secesso", "VenturaHR");
					},
					(error: HttpErrorResponse) => {								
						if (error.status == 404) {
							let bodyError = error.error;
							this.toastr.error(bodyError.message, "VenturaHR");	
						}				
						else{
							this.toastr.error("Erro inesperado", "VenturaHR");
						}
					}),

			)
	}

	public logout(): void {
		sessionStorage.clear();
		this.router.navigate(['/access']);
		this.toastr.success("Logout realizado com sucesso", "VenturaHR");	
	}
}
