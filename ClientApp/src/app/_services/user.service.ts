import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl: string = 'https://localhost:44345/api/'

  constructor(private http: HttpClient) { }


  getCurrentUser() {
    return this.http.get(this.baseUrl + 'users');
  }

  getUsersFilter(name?, email?) {
    return this.http.get(this.baseUrl + 'users/filter', {
      params: {
        name: name ? name : '',
        email: email ? email : ''
      }
    });
  }

  getUserById(id) {
    return this.http.get(this.baseUrl + 'users/' + id);
  }

  getUserByEmail(email) {
    return this.http.get(this.baseUrl + 'users/get-id/' + email)
  }

  updatePhoto(photo) {
    return this.http.put(this.baseUrl + 'users/add-photo', photo, { responseType: 'blob' });
  }

  deletePhoto() {
    return this.http.put(this.baseUrl + 'users/delete-photo', null,);
  }
}
