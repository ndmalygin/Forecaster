# Forecaster 🌤️

[![Language](https://shields.io)](https://microsoft.com)
[![Framework](https://shields.io)](https://microsoft.com)
[![Infrastructure](https://shields.io)](https://rabbitmq.com)
[![License: MIT](https://shields.io)](LICENSE)

An enterprise-grade, microservice-based system designed to fetch weather data via the external **OpenWeather API**, process it asynchronously via a message broker, store it reliably, and visualize historical data in real-time.

---

## 🏗️ Architecture Overview

The system is designed following the **Event-Driven Architecture (EDA)** pattern to decouple the ingestion of data from its consumption and processing.


### 🧩 Microservice Components

1. **`ForecastsCollector`**: A worker service tasked with polling scheduled weather data from the OpenWeather API and publishing raw events onto the message broker.
2. **`ForecastsRabbitMQDispatcher`**: An infrastructure layer responsible for handling connection lifecycles, exchanges, topologies, and publishing mechanisms for **RabbitMQ**.
3. **`ForecastsServiceApp`**: The core backend business logic service. It consumes transactions from RabbitMQ, performs validation/transformation, and persists documents to **MongoDB**.
4. **`ForecastsWebApp`**: A reactive user interface built with **Blazor Server** that enables live visualization and monitoring of collected weather forecasts.
5. **`ForecastsCommon`**: A shared library containing domain models, event contracts, and cross-cutting helpers used across all microservices.
6. **`ForecastsConsumerTest`**: Integration and unit-testing suite focused on simulating pipeline loads and verifying consumer behaviors.

---

## ⚡ Key Architectural Highlights

* **Asynchronous Decoupling:** Heavy heavy-lifting API calls and storage logic are disconnected using RabbitMQ, protecting the system against high-latency drops.
* **NoSQL Persistence:** Utilizes MongoDB to effortlessly store flexible, nested JSON weather matrices without rigid relational object-mapping (ORM) bottlenecks.
* **Container Ready:** Fully equipped with standalone `Dockerfile` configurations (`Dockerfile.forecasterservice` & `Dockerfile.forecasterweb`), making it ready for cloud-native orchestration (Kubernetes/Docker Compose).
* **Modern Web Tech:** Leverages Blazor Server to achieve fast, single-page-application (SPA) data streaming without writing heavy Javascript boilerplate code.

---

## 💻 Local Development & Setup

### Prerequisites
* **.NET SDK 10.0** or later
* **RabbitMQ Server** (running with default AMQP port `5672`)
* **MongoDB Instance** (running on port `27017`)
* **OpenWeather API Key** (configured via user-secrets or environment variables)

### Step-by-Step Launch

1. Clone the repository:
   ```bash
   git clone https://github.com
   cd Forecaster
   ```

2. Configure your OpenWeather API settings in the `ForecastsCollector` configuration settings or environment variables.

3. Run the solution using the primary `.sln` configuration file via Visual Studio / JetBrains Rider or via CLI:
   ```bash
   dotnet restore Forecaster.sln
   dotnet build Forecaster.sln
   ```

---

## 🐳 Docker Deployment

The application features microservice container templates for cloud-scale packaging:

* To build the core backend processing service:
  ```bash
  docker build -f Dockerfile.forecasterservice -t forecaster-service .
  ```
* To build the Blazor visualization web app:
  ```bash
  docker build -f Dockerfile.forecasterweb -t forecaster-web .
  ```
## 🐳 Please READ Setup.txt for details !!!
---

## 📝 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


