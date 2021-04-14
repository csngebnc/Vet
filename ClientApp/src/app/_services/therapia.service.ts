import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TherapiaService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addTherapia(model: any) {
    return this.http.post(this.baseUrl + 'therapia', model);
  }

  updateTherapia(model: any) {
    return this.http.put(this.baseUrl + 'therapia', model);
  }

  changeState(id) {
    return this.http.put(this.baseUrl + 'therapia/state/' + id, null);
  }

  deleteTherapia(id) {
    return this.http.delete(this.baseUrl + 'therapia/' + id);
  }

  getTherapiaById(id) {
    return this.http.get(this.baseUrl + 'therapia/' + id);
  }

  getTherapias() {
    return this.http.get(this.baseUrl + 'therapia')
  }
}
