import type { Component } from 'solid-js';

import logo from './logo.svg';
import styles from './App.module.css';
import { Button } from '@suid/material';
import { createStore } from "solid-js/store";
import { SolidApexCharts } from 'solid-apexcharts';
const App: Component = () => {
  const [options] = createStore({
    chart: {
      id: 'solidchart-example',
    },
    xaxis: {
      categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998],
    },
  });
  const [series] = createStore({
    list: [
      {
        name: 'series-1',
        data: [30, 40, 35, 50, 49, 60, 70, 91],
      },
    ]
  });
  return (
    <div class={styles.App}>
      <header class={styles.header}>
        <SolidApexCharts width="500" type="bar" options={options} series={series.list} />;
        <Button variant="contained">Hello wrold</Button>
        <img  src={logo} class={styles.logo} alt="logo" />
        <p class={styles.postcsstest}>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          class={styles.link}
          href="https://github.com/solidjs/solid"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn Solid
        </a>
      </header>
    </div>
  );
};

export default App;
