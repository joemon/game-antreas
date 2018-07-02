import React, { Component } from "react";

import Header from "../Header/Header";
import Footer from "../Footer/Footer";

const App = ({ children }) => (
  <div>
    <Header />
    <div className="content">
      <main className="main-view">{children}</main>
    </div>
    <Footer />
  </div>
);

export default App;
