import http from 'k6/http';
import { sleep, check } from 'k6';

// --- Load profile: how much traffic we generate ---
export const options = {
  stages: [
    // Warm up
    { duration: '10s', target: 5 },
    // Small load
    { duration: '20s', target: 10 },
    // Peak
    { duration: '20s', target: 20 },
    // Ramp down
    { duration: '10s', target: 0 },
  ],
  thresholds: {
    // 95% of requests should be faster than 800ms
    http_req_duration: ['p(95)<800'],
    // Error rate should be < 1%
    http_req_failed: ['rate<0.01'],
  },
};

export default function () {
  const baseUrl = __ENV.BASE_URL || 'http://localhost:5000';

  const res = http.get(baseUrl);

  check(res, {
    'status is 200': (r) => r.status === 200,
  });

  // Small think time between requests
  sleep(1);
}
