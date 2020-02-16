import { Component, OnInit } from '@angular/core';
import { Country } from '../country';
import { CountryApiService } from '../services/country-api.service';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.scss']
})
export class CountriesComponent implements OnInit {
  selectedCountry: Country;
  countryApiService: CountryApiService;
  countries: Array<any>;
  // current page of items
  pageOfCountries: Array<any>;
  numberOfCountriesOnPage: number;

  constructor(countryApiService: CountryApiService) {
    this.countryApiService = countryApiService;
    this.numberOfCountriesOnPage = 5;
  }

  ngOnInit() {
    this.getCountries();
  }

  getCountries(): void {
    this.countryApiService.getCountries()
      .subscribe(data => this.countries = data);
  }

  onSelect(selectedCountry: Country): void {
    this.selectedCountry = selectedCountry;
  }
  
  onChangePage(pageOfCountries: Array<any>) {
    // update current page of items
    this.pageOfCountries = pageOfCountries;
  }
}
