import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

// Set the http options
const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};

@Injectable({
  providedIn: 'root'
})

export class CountryApiService {
  constructor(private httpClient: HttpClient) { }

  public getCountries(): Observable<any> {
    return this.httpClient.get(`${environment.baseApiUrl}/country`, httpOptions);
  }
}
