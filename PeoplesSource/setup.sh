echo "Updating apt-get #######################################"
apt-get update
echo "Installing packages ####################################"
apt-get install -y nano squid apache2-utils
echo "setting password for the user ##########################"
touch /etc/squid3/passwd
chown proxy /etc/squid3/passwd
htpasswd /etc/squid3/passwd h247hustlin
mv /etc/squid3/squid.conf /etc/squid3/squid.conf.original
chmod a-w /etc/squid3/squid.conf.original
touch /etc/squid3/squid.conf
echo "Configuring squid.conf ####################################"
cat >> /etc/squid3/squid.conf << EOF
http_port 7585
cache deny all
hierarchy_stoplist cgi-bin ?
#line added by khawar
access_log none
#access_log /var/log/squid3/access.log squid
#cache_log /var/log/squid3/cache.log
cache_store_log none
cache_log /dev/null

refresh_pattern ^ftp: 1440 20% 10080
refresh_pattern ^gopher: 1440 0% 1440
refresh_pattern -i (/cgi-bin/|\?) 0 0% 0
refresh_pattern . 0 20% 4320

#acl manager proto cache_object
#acl localhost src 127.0.0.1/32 ::1
#acl to_localhost dst 127.0.0.0/8 0.0.0.0/32 ::1

auth_param basic program /usr/lib/squid3/basic_ncsa_auth /etc/squid3/passwd
auth_param basic children 5
auth_param basic realm Welcome. Enter ID and Password. Need help? Contact Wraddle on Skype
auth_param basic credentialsttl 2 hours

acl SSL_ports port 443
acl SSL_ports port 5656 # SolusVM SSL
acl Safe_ports port 5353 # SolusVM without SSL
acl Safe_ports port 80 # http
acl Safe_ports port 21 # ftp
acl Safe_ports port 443 # https
acl Safe_ports port 70 # gopher
acl Safe_ports port 210 # wais
acl Safe_ports port 1025-65535 # unregistered ports
acl Safe_ports port 280 # http-mgmt
acl Safe_ports port 488 # gss-http
acl Safe_ports port 591 # filemaker
acl Safe_ports port 777 # multiling http
acl CONNECT method CONNECT
http_access allow manager localhost
http_access deny manager

http_access deny !Safe_ports

http_access deny CONNECT !SSL_ports
acl password proxy_auth REQUIRED
http_access allow localhost
http_access allow password
http_access deny all

forwarded_for off
request_header_access Allow allow all
request_header_access Authorization allow all
request_header_access WWW-Authenticate allow all
request_header_access Proxy-Authorization allow all
request_header_access Proxy-Authenticate allow all
request_header_access Cache-Control allow all
request_header_access Content-Encoding allow all
request_header_access Content-Length allow all
request_header_access Content-Type allow all
request_header_access Date allow all
request_header_access Expires allow all
request_header_access Host allow all
request_header_access If-Modified-Since allow all
request_header_access Last-Modified allow all
request_header_access Location allow all
request_header_access Pragma allow all
request_header_access Accept allow all
request_header_access Accept-Charset allow all
request_header_access Accept-Encoding allow all
request_header_access Accept-Language allow all
request_header_access Content-Language allow all
request_header_access Mime-Version allow all
request_header_access Retry-After allow all
request_header_access Title allow all
request_header_access Connection allow all
request_header_access Proxy-Connection allow all
request_header_access User-Agent allow all
request_header_access Cookie allow all
request_header_access All deny all

EOF
echo "Restarting squid ####################################"
service squid3 restart
echo "Configuring IPv4 and IPv6 ##########################"
echo 1 > /proc/sys/net/ipv6/conf/all/disable_ipv6
printf "## lines added by squid setup script \n" >> /etc/sysctl.conf
printf "net.ipv6.conf.all.disable_ipv6 = 1 \n" >> /etc/sysctl.conf
printf "net.ipv6.conf.default.disable_ipv6 = 1 \n" >> /etc/sysctl.conf
printf "net.ipv6.conf.lo.disable_ipv6 = 1 \n" >> /etc/sysctl.conf


sysctl -p
echo "Finished ############################################"
