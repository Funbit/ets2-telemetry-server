parse_yaml2() {
	local prefix=$2
	local s
	local w
	local fs
	s='[[:space:]]*'
	w='[a-zA-Z0-9_]*'
	fs="$(echo @|tr @ '\034')"
	sed -ne "s|^\($s\)\($w\)$s:$s\"\(.*\)\"$s\$|\1$fs\2$fs\3|p" \
		-e "s|^\($s\)\($w\)$s[:-]$s\(.*\)$s\$|\1$fs\2$fs\3|p" "$1" |
	awk -F"$fs" '{
		indent = length($1)/2;
		if (length($2) == 0) { conj[indent]="+";} else {conj[indent]="";}
		vname[indent] = $2;
		for (i in vname) {if (i > indent) {delete vname[i]}}
		if (length($3) > 0) {
			vn=""; for (i=0; i<indent; i++) {vn=(vn)(vname[i])("_")}
			printf("%s%s%s%s=(\"%s\")\n", "'"$prefix"'",vn, $2, conj[indent-1],$3);
		}
	}' | sed 's/_=/+=/g'
}

echo=printf

eval $(parse_yaml2 .travis.yml config_)
#parse_yaml2 .travis.yml

installlength=${#config_install[@]}

for (( i=0; i<${installlength}; i++ )); do
	printf "\033[37minstalling %d/%d \033[1m%s\033[21m:\033[0m\n" $(($i + 1)) ${installlength} "${config_install[$i]}"

	CMD=${config_install[$i]}

	if [[ ${CMD:0:1} == "(" ]] ; then
		sh -c "${config_install[$i]}"
	else
		${config_install[$i]}
	fi

	if [ $? -eq 0 ] ; then
		printf "%s \033[92m\033[1m%s\033[21m\033[0m\n" "$CMD" "OK"
	else
		printf "%s \033[91m\033[1m%s\033[21m\033[0m\n" "$CMD" "FAILED"
		exit 1
	fi
done

scriptlength=${#config_script[@]}

for (( i=0; i<${scriptlength}; i++ )); do
	printf "\033[36mtesting %d/%d \033[1m%s\033[21m:\033[0m\n" $(($i + 1)) ${scriptlength} "${config_script[$i]}"

	CMD=${config_script[$i]}

	if [[ ${CMD:0:1} == "(" ]] ; then
		sh -c "${config_script[$i]}"
	else
		${config_script[$i]}
	fi

	if [ $? -eq 0 ] ; then
		printf "%s \033[92m\033[1m%s\033[21m\033[0m\n" "${config_script[$i]}" "OK"
	else
		printf "%s \033[91m\033[1m%s\033[21m\033[0m\n" "${config_script[$i]}" "FAILED"
		exit 1
	fi

done

after_scriptlength=${#config_after_script[@]}

for (( i=0; i<${after_scriptlength}; i++ )); do
	printf "\033[35mcleaning up %d/%d \033[1m%s\033[21m\033[0m: %s\n" $(($i + 1)) ${after_scriptlength} "${config_after_script[$i]}"

	${config_after_script[$i]}

	if [ $? -eq 0 ] ; then
		printf "%s \033[92m\033[1m%s\033[21m\033[0m\n" "${config_after_script[$i]}" "OK"
	else
		printf "%s \033[91m\033[1m%s\033[21m\033[0m\n" "${config_after_script[$i]}" "FAILED"
		exit 1
	fi

done
